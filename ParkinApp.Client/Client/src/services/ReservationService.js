import axios from "axios";
import API_ENDPOINT from '../config';

const API_URL = `${API_ENDPOINT}/api/Reservations/`;

const createReservation = async (parkingSpotId, token) => {
    try {
        const response = await axios.post(`${API_URL}create`, { parkingSpotId }, { headers: { Authorization: `Bearer ${token}` } });
        return response.data;
    } catch (error) {
        console.error("Error while creating reservation:", error);
        console.error("Error details:", error.response);
        console.error("Request headers:", error.config.headers);
        throw error;
    }
};

const cancelReservation = async (token) => {
    console.log("Token in ReservationService:", token);
    const headers = { Authorization: `Bearer ${token}` };
    try {
        const response = await axios.delete(API_URL + "cancel", { headers: headers });
        return response.data;
    } catch (error) {
        console.error("Error while cancelling reservation:", error);
        console.error("Error details:", error.response);
        throw error;
    }
};


export { createReservation, cancelReservation };
