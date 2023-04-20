import React from "react";
import { Badge } from "react-bootstrap";

const Counter = ({ occupiedSpots, freeSpots }) => {
    return (
        <div>
            <h3>
                Occupied Spots: <Badge bg="danger">{occupiedSpots}</Badge>
            </h3>
            <h3>
                Free Spots: <Badge bg="success">{freeSpots}</Badge>
            </h3>
        </div>
    );
};

export default Counter;
