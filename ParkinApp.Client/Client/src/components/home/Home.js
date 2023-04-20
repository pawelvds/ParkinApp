import React, { useState, useEffect } from "react";
import ParkingSpotService from "../../services/ParkingSpotService";
import { Container, Row } from "react-bootstrap";
import OccupiedFreeSpots from "./Counter";
import ParkingSpotCard from "./ParkingSpotCard";
import CancelReservation from "./CancelReservation";
import AuthService from "../../services/AuthService";

const Home = ({ currentUser, token }) => {
    const [parkingSpots, setParkingSpots] = useState([]);
    const [occupiedSpots, setOccupiedSpots] = useState(0);
    const [freeSpots, setFreeSpots] = useState(0);
    const [message, setMessage] = useState(null);
    const [userReservation, setUserReservation] = useState(null);

    const refreshSpots = () => {
        ParkingSpotService.getParkingSpots()
            .then((response) => setParkingSpots(response.data))
            .catch((error) => console.error(error));
    };

    useEffect(() => {
        refreshSpots();
    }, []);

    useEffect(() => {
        const occupied = parkingSpots.filter((spot) => spot.reserved).length;
        const free = parkingSpots.length - occupied;

        setOccupiedSpots(occupied);
        setFreeSpots(free);
    }, [parkingSpots]);

    useEffect(() => {
        const interval = setInterval(() => {
            refreshSpots();
        }, 30000);
        return () => clearInterval(interval);
    }, []);

    useEffect(() => {
        if (currentUser) {
            const userReservedSpot = parkingSpots.find(
                (spot) => spot.reserved && spot.reservedBy.id === currentUser.id
            );

            if (userReservedSpot) {
                setUserReservation(userReservedSpot.id);
            } else {
                setUserReservation(null);
            }
        }
    }, [parkingSpots, currentUser]);

    const isUserSpotReserved = (parkingSpot) => {
        if (currentUser) {
            return parkingSpot.reserved && parkingSpot.reservedBy.id === currentUser.id;
        }
        return false;
    };

    const getUserReservedSpotId = () => {
        if (currentUser && parkingSpots.length > 0) {
            const reservedSpot = parkingSpots.find(
                (spot) => spot.reserved && spot.reservedBy === currentUser.username
            );
            return reservedSpot ? reservedSpot.id : null;
        }
        return null;
    };

    const handleMessage = (msg) => {
        setMessage(msg);
        setTimeout(() => {
            setMessage(null);
        }, 3000);
    };


    return (
        <Container>
            {!currentUser ? (
                <>
                    <h2>Welcome to ParkinApp</h2>
                    <p>
                        To see available parking spots and make reservations, please{" "}
                        <a href="/login">log in</a> or <a href="/register">sign up</a>.
                    </p>
                    <OccupiedFreeSpots occupiedSpots={occupiedSpots} freeSpots={freeSpots} />
                </>
            ) : (
                <>
                    <h2>Parking Spots:</h2>
                    {getUserReservedSpotId() ? (
                        <p>
                            Hi {currentUser.username}! You have reserved spot number {getUserReservedSpotId()}.
                        </p>
                    ) : (
                        <p>Hi {currentUser.username}! You do not have any reservations. Choose an available spot.</p>
                    )}

                    {message && (
                        <div className={`alert ${message.type}`}>{message.content}</div>
                    )}
                    <Row>
                        {parkingSpots.map((parkingSpot) => (
                            <ParkingSpotCard
                                key={parkingSpot.id}
                                parkingSpot={parkingSpot}
                                refreshSpots={refreshSpots}
                                handleMessage={handleMessage}
                                setUserReservation={setUserReservation}
                                currentUser={currentUser}
                                token={token}
                                isUserSpotReserved={isUserSpotReserved}
                            />
                        ))}
                    </Row>
                    <CancelReservation
                        refreshSpots={refreshSpots}
                        handleMessage={handleMessage}
                        setUserReservation={setUserReservation}
                        userReservation={userReservation}
                    />
                </>
            )}
        </Container>
    );
};

export default Home;