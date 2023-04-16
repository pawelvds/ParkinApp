import React from "react";
import { cancelReservation } from "../../services/ReservationService";

const CancelReservationButton = ({ token, refreshSpots }) => {
    const handleClick = async () => {
        try {
            const result = await cancelReservation(token);
            console.log("Reservation cancelled:", result);
            refreshSpots();
        } catch (error) {
            console.error("Error while cancelling reservation:", error);
        }
    };

    return <button onClick={handleClick}>Cancel Reservation</button>;
};

export default CancelReservationButton;
