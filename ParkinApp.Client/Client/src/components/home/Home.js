import React, { useState, useEffect } from "react";
import ParkingSpotService from "../../services/ParkingSpotService";
import { Container, Row } from "react-bootstrap";
import OccupiedFreeSpots from "./Counter";
import ParkingSpotCard from "./ParkingSpotCard";
import YourReservations from "./CancelReservation";

const Home = ({ currentUser }) => {
    const [parkingSpots, setParkingSpots] = useState([]);
    const [occupiedSpots, setOccupiedSpots] = useState(0);
    const [freeSpots, setFreeSpots] = useState(0);

    const refreshSpots = () => {
        ParkingSpotService.getParkingSpots()
            .then((response) => setParkingSpots(response.data))
            .catch((error) => console.error(error));
    };

    useEffect(() => {
        refreshSpots();
    }, []);

    useEffect(() => {
        const occupied = parkingSpots.filter(spot => spot.reserved).length;
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
                    <Row>
                        {parkingSpots.map((parkingSpot) => (
                            <ParkingSpotCard
                                key={parkingSpot.id}
                                parkingSpot={parkingSpot}
                                refreshSpots={refreshSpots}
                            />
                        ))}
                    </Row>
                    <YourReservations refreshSpots={refreshSpots} />
                </>
            )}
        </Container>
    );
};

export default Home;
