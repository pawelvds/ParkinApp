import React, { useState, useEffect } from "react";
import ParkingSpotService from "../../services/ParkingSpotService";
import { Container, Row, Col, Card, Button } from "react-bootstrap";

const Home = ({ currentUser }) => {
    const [parkingSpots, setParkingSpots] = useState([]);

    useEffect(() => {
        ParkingSpotService.getParkingSpots()
            .then((response) => setParkingSpots(response.data))
            .catch((error) => console.error(error));
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
                                            Reserved by:
                                        </Card.Text>
                                        <Button variant="primary">Reserve</Button>
                                    </Card.Body>
                                </Card>
                            </Col>
                        ))}
                    </Row>
                </>
            )}
        </Container>
    );
};

export default Home;