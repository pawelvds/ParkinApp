import React, { useState } from "react";
import { Button } from "react-bootstrap";
import { createReservation } from "../../services/ReservationService";
import AuthService from "../../services/AuthService";

const ReservationForm = ({ parkingSpotId }) => {
    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

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
            const response = await createReservation(parkingSpotId, token);
            setSuccess(true);
            console.log("Reservation created:", response);
        } catch (error) {
            setError("Error creating reservation.");
            console.error("Error details:", error.response);
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            {error && <div className="alert alert-danger">{error}</div>}
            {success && (
                <div className="alert alert-success">Reservation created successfully!</div>
            )}
            <Button variant="primary" type="submit" disabled={loading}>
                {loading ? "Reserving..." : "Reserve"}
            </Button>
        </form>
    );
};

export default ReservationForm;
