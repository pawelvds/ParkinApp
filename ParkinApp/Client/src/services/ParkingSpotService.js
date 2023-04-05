import React from 'react';
import axios from 'axios';
import API_ENDPOINT from "../config";

const getParkingSpots = () => {
    return axios.get(API_ENDPOINT + '/parking-spots');
}

const ParkingSpotService = {
    getParkingSpots
}

export default ParkingSpotService;