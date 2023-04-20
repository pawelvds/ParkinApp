import axios from "axios";
import jwt_decode from "jwt-decode";
import API_ENDPOINT from "../config";

const isTokenExpired = (token) => {
    if (!token) {
        console.error("No token provided");
        return true;
    }
    try {
        const decodedToken = jwt_decode(token);
        const expired = decodedToken.exp < Date.now() / 1000;
        return expired;
    } catch (error) {
        console.error("Error checking access token expiration:", error);
        return true;
    }
};

const refreshAccessToken = async (refreshToken) => {
    try {
        const response = await axios.post(API_ENDPOINT + "/api/User/refresh-token", { refreshToken: refreshToken });

        if (response.data.accessToken) {
            const user = {
                ...JSON.parse(localStorage.getItem("user")),
                accessToken: response.data.accessToken,
                refreshToken: response.data.refreshToken,
            };
            localStorage.setItem("user", JSON.stringify(user));
            return user;
        }
    } catch (error) {
        console.error("Error refreshing access token:", error);
        throw error;
    }
};

const requestWithAuth = async (url, options = {}) => {
    const user = JSON.parse(localStorage.getItem("user"));
    if (!user) {
        throw new Error("User not found");
    }

    if (isTokenExpired(user.accessToken)) {
        try {
            await refreshAccessToken(user.refreshToken);
        } catch (error) {
            console.error("Error refreshing access token:", error);
            throw error;
        }
    }

    const updatedUser = JSON.parse(localStorage.getItem("user"));
    const authOptions = {
        ...options,
        headers: {
            ...options.headers,
            Authorization: `Bearer ${updatedUser.accessToken}`,
        },
    };

    return axios(url, authOptions);
};

const getCurrentUser = () => {
    const user = JSON.parse(localStorage.getItem("user"));
    return user;
};

const getAccessToken = () => {
    const user = getCurrentUser();

    if (user && user.accessToken) {
        return user.accessToken;
    }

    return null;
};

const login = async (userName, password) => {
    try {
        const response = await axios.post(API_ENDPOINT + '/api/User/login', {
            userName,
            password,
        });

        if (response.data.accessToken) {
            localStorage.setItem('user', JSON.stringify(response.data));
        }

        return response.data;
    } catch (error) {
        console.error('Error logging in:', error);
        console.error('Error response:', error.response);
        console.error('Error request:', error.request);
        console.error('Error response data:', error.response.data);
        throw error;
    }
};

const logout = () => {
    return new Promise((resolve) => {
        localStorage.removeItem("user");
        resolve();
    });
};

const register = async (userName, password) => {
    try {
        const response = await axios.post(API_ENDPOINT + "/api/User/register", {
            userName,
            password,
        });

        if (response.data.accessToken) {
            localStorage.setItem("user", JSON.stringify(response.data));
        }

        return response.data;
    } catch (error) {
        console.error("Error registering user:", error);
        console.error("Error response:", error.response);
        console.error("Error request:", error.request);
        console.error("Error response data:", error.response.data);
        throw error;
    }
};


const AuthService = {
    isTokenExpired,
    refreshAccessToken,
    requestWithAuth,
    getCurrentUser,
    getAccessToken,
    login,
    logout,
    register,
};

export default AuthService;
