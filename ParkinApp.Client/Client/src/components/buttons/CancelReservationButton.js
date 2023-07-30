// CancelReservationButton.js
import React, { useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import { cancelReservation } from "../../services/ReservationService";
import AuthService from "../../services/AuthService";

const CancelReservationButton = ({
                                     refreshSpots,
                                     handleMessage,
                                     setUserReservation,
                                     userReservation,
                                 }) => {
    const [loading, setLoading] = useState(false);
    const [isButtonDisabled, setIsButtonDisabled] = useState(true);

    useEffect(() => {
        setIsButtonDisabled(loading || userReservation === null);
    }, [loading, userReservation]);

    const handleClick = async () => {
        setLoading(true);

        const token = AuthService.getAccessToken();
        if (!token) {
            handleMessage({
                type: "alert-danger",
                content: "Missing access token.",
            });
            setLoading(false);
            return;
        }

        try {
            const response = await cancelReservation(token);
            setUserReservation(null);
            handleMessage                ({
                type: "alert-success",
                content: "Reservation cancelled successfully!",
            });
            refreshSpots();
        } catch (error) {
            handleMessage({
                type: "alert-danger",
                content: "Error cancelling reservation.",
            });
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Button
                variant="primary"
                onClick={handleClick}
                disabled={isButtonDisabled}
            >
                {loading ? "Cancelling..." : "Cancel Reservation"}
            </Button>
        </div>
    );
};

export default CancelReservationButton;

