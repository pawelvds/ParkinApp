import React, { useState, useEffect } from "react";
import parkingSpotService from "../../services/ParkingSpotService";

const Home = () => {
    const [parkingSpots, setParkingSpots] = useState([]);

    useEffect(() => {
        parkingSpotService.getParkingSpots()
            .then(response => setParkingSpots(response.data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div>
            <h2>Parking Spots:</h2>
            <ul>
                {parkingSpots.map(parkingSpot => (
                    <li key={parkingSpot.id}>{parkingSpot.name}</li>
                ))}
            </ul>
        </div>
    );
};

export default Home;