import React, { useState, useEffect } from "react";
import ParkingSpotService from "../../services/ParkingSpotService";
import { Container, Row, Col, Card } from "react-bootstrap";
import ReservationForm from "../forms/ReservationForm";
import CancelReservationButton from "../buttons/CancelReservationButton";

const Home = ({ currentUser }) => {
    const [parkingSpots, setParkingSpots] = useState([]);

    const refreshSpots = () => {
        ParkingSpotService.getParkingSpots()
            .then((response) => setParkingSpots(response.data))
            .catch((error) => console.error(error));
    };

    useEffect(() => {
        refreshSpots();
    }, []);

    const [occupiedParkingSpots, setOccupiedParkingSpots] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const occupiedSpotsPromises = parkingSpots.map(async (spot) => {
                    const occupiedSpotData = await ParkingSpotService.getOccupiedParkingSpots(spot.id);
                    return { ...spot, ...occupiedSpotData };
                });

                const updatedSpots = await Promise.all(occupiedSpotsPromises);
                setParkingSpots(updatedSpots);
            } catch (error) {
                console.error('Error fetching occupied parking spots:', error);
            }
        };

        if (parkingSpots.length > 0) {
            fetchData();
        }
    }, [parkingSpots]);

    return (
        <Container>
            {!currentUser ? (
                <>
                    <h2>Welcome to ParkinApp</h2>
                    <p>
                        To see available parking spots and make reservations, please{" "}
                        <a href="/login">log in</a> or <a href="/register">sign up</a>.
                    </p>
                </>
            ) : (
                <>
                    <h2>Parking Spots:</h2>
                    <Row>
                        {parkingSpots.map((parkingSpot) => (
                            <Col md={3} key={parkingSpot.id}>
                                <Card>
                                    <Card.Body>
                                        <Card.Title>{parkingSpot.name}</Card.Title>
                                        <Card.Text>
                                            Spot ID: {parkingSpot.id} <br />
                                            Time Zone: {parkingSpot.spotTimeZone} <br />
                                            Reserved by: {parkingSpot.reservedBy ? parkingSpot.reservedBy : "Not reserved"}
                                        </Card.Text>
                                        <ReservationForm
                                            parkingSpotId={parkingSpot.id}
                                            reserved={parkingSpot.reserved}
                                            refreshSpots={refreshSpots}
                                        />
                                    </Card.Body>
                                </Card>
                            </Col>
                        ))}
                    </Row>
                    <div style={{ marginTop: "20px" }}>
                        <h3>Your Reservations:</h3>
                        <CancelReservationButton
                            refreshSpots={refreshSpots}
                        />
                    </div>
                </>
            )}
        </Container>
    );
};

export default Home;
