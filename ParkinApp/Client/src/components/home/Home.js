import React, { useState, useEffect } from "react";
import ParkingSpotService from "../../services/ParkingSpotService"; 

const Home = () => {
    const [parkingSpots, setParkingSpots] = useState([]);

    useEffect(() => {
        ParkingSpotService.getParkingSpots() 
            .then(response => setParkingSpots(response.data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div>
            <h2>Parking Spots:</h2>
            <ul>
                {parkingSpots.map(parkingSpot => (
                    <li key={parkingSpot.id}>Spot ID: {parkingSpot.id} - Time Zone: {parkingSpot.spotTimeZone}</li>
                ))}
            </ul>
        </div>

    );
};

export default Home;
