import React, { useState } from "react";
import { Button } from "react-bootstrap";
import { createReservation } from "../../services/ReservationService";
import AuthService from "../../services/AuthService";

const ReservationForm = ({
                             parkingSpotId,
                             reserved,
                             refreshSpots,
                             setUserReservation,
                             handleMessage,
                             currentUser,
                             token,
                         }) => {
    const [loading, setLoading] = useState(false);

    const handleReserve = async () => {
        setLoading(true);

        if (!token) {
            handleMessage({
                type: "alert-danger",
                content: "Missing access token.",
            });
            setLoading(false);
            return;
        }

        try {
            await createReservation(parkingSpotId, token);
            setUserReservation(parkingSpotId);
            handleMessage({
                type: "alert-success",
                content: "Reservation created successfully!",
            });
            refreshSpots();
        } catch (error) {
            handleMessage({
                type: "alert-danger",
                content: "Error creating reservation.",
            });
        } finally {
            setLoading(false);
        }
    };

    return (
        <Button
            variant="primary"
            disabled={reserved || loading}
            onClick={handleReserve}
        >
            {reserved ? "Reserved" : loading ? "Reserving..." : "Reserve"}
        </Button>
    );
};

export default ReservationForm;
