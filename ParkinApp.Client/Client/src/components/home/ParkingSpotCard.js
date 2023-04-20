import React from "react";
import { Col, Card } from "react-bootstrap";
import ReservationForm from "../forms/ReservationForm";

const ParkingSpotCard = (props) => {
    const { parkingSpot, refreshSpots, handleMessage, setUserReservation, currentUser, token} = props;

    return (
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
                        setUserReservation={setUserReservation}
                        handleMessage={handleMessage}
                        currentUser={currentUser}
                        token={token}
                    />
                </Card.Body>
            </Card>
        </Col>
    );
};

export default ParkingSpotCard;
