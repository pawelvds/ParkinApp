import axios from 'axios';
import API_ENDPOINT from "../config";

const getParkingSpots = () => {
    return axios.get(API_ENDPOINT + '/api/ParkingSpots');
}

const getOccupiedParkingSpots = async (parkingSpotId) => {
    const response = await axios.get(API_ENDPOINT + `/api/Reservations/occupied/${parkingSpotId}`);
    return response.data;
}

const ParkingSpotService = {
    getParkingSpots,
    getOccupiedParkingSpots
};

export default ParkingSpotService;
