import React from "react";
import { Link, useNavigate } from "react-router-dom";
import AuthService from "../../services/AuthService";

const Navbar = ({ currentUser, onLogout }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        AuthService.logout().then(() => {
            onLogout();
            navigate("/home");
        });
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <div className="container-fluid">
                <Link to={"/"} className="navbar-brand">
                    ParkinApp
                </Link>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNav"
                    aria-controls="navbarNav"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNav">
                    {currentUser ? (
                        <div className="navbar-nav ms-auto">
                            <li className="nav-item">
                                <Link to="/logout" className="nav-link" onClick={handleLogout}>
                                    Logout
                                </Link>
                            </li>
                        </div>
                    ) : (
                        <div className="navbar-nav ms-auto">
                            <li className="nav-item">
                                <Link to={"/login"} className="nav-link">
                                    Login
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link to={"/register"} className="nav-link">
                                    Register
                                </Link>
                            </li>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
