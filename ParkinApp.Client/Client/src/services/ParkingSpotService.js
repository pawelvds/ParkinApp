import axios from 'axios';
import API_ENDPOINT from "../config";

const getParkingSpots = () => {
    return axios.get(API_ENDPOINT + '/api/ParkingSpots');
}

const ParkingSpotService = {
    getParkingSpots
};

export default ParkingSpotService;