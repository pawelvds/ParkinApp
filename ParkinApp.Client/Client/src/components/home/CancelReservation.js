import React from "react";
import CancelReservationButton from "../buttons/CancelReservationButton";

const CancelReservation = ({
                               refreshSpots,
                               handleMessage,
                               setUserReservation,
                               userReservation,
                           }) => {
    return (
        <div style={{ marginTop: "20px" }}>
            <h3>Your Reservations:</h3>
            <CancelReservationButton
                refreshSpots={refreshSpots}
                handleMessage={handleMessage}
                setUserReservation={setUserReservation}
                userReservation={userReservation}
            />
        </div>
    );
};

export default CancelReservation;
