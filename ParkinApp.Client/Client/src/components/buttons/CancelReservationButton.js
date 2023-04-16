import React, { useState } from "react";
import { Button } from "react-bootstrap";
import { cancelReservation } from "../../services/ReservationService";
import AuthService from "../../services/AuthService";

const CancelReservationButton = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);

    const handleClick = async () => {
        setLoading(true);
        setError(null);
        setSuccess(false);

        const token = AuthService.getAccessToken();
        if (!token) {
            setError("Missing access token.");
            setLoading(false);
            return;
        }

        try {
            const response = await cancelReservation(token);
            setSuccess(true);
            console.log("Reservation cancelled:", response);
        } catch (error) {
            setError("Error cancelling reservation.");
            console.error("Error details:", error.response);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            {error && <div className="alert alert-danger">{error}</div>}
            {success && (
                <div className="alert alert-success">
                    Reservation cancelled successfully!
                </div>
            )}
            <Button variant="primary" onClick={handleClick} disabled={loading}>
                {loading ? "Cancelling..." : "Cancel Reservation"}
            </Button>
        </div>
    );
};

export default CancelReservationButton;
