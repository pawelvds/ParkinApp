import React from "react";
import CancelReservationButton from "../buttons/CancelReservationButton";

const CancelReservation = ({ refreshSpots }) => {
    return (
        <div style={{ marginTop: "20px" }}>
            <h3>Your Reservations:</h3>
            <CancelReservationButton refreshSpots={refreshSpots} />
        </div>
    );
};

export default CancelReservation;
