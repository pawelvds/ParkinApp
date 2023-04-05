import React, { useState } from "react";
import AuthService from "../../services/AuthService";
import { useNavigate } from "react-router-dom";

const Register = () => {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");

    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await AuthService.register(userName, password).then(
                () => {
                    navigate("/home");
                    window.location.reload();
                },
                (error) => {
                    console.log(error);
                }
            );
        } catch (err) {
            console.log(err);
        }
    };

    return (
        <div>
            <form onSubmit={handleRegister}>
                <h3>Sign up</h3>
                <input
                    type="text"
                    placeholder="user name"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                />
                <input
                    type="password"
                    placeholder="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;