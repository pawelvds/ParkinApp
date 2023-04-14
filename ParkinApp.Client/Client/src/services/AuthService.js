import axios from "axios";
import jwt from 'jsonwebtoken';
import API_ENDPOINT from "../config";

const isTokenExpired = (token) => {
    console.log('Checking if access token is expired');
    if (!token) {
        console.error('No token provided');
        return true;
    }
    try {
        console.log('Token to decode:', token);
        const decodedToken = jwt.decode(token);
        const expired = decodedToken.exp < Date.now() / 1000;
        if (expired) {
            console.log('Access token is expired');
        } else {
            console.log('Access token is not expired');
        }
        return expired;
    } catch (error) {
        console.error('Error checking access token expiration:', error);
        return true;
    }
};

const isRefreshTokenExpired = (token) => {
    const expired = token && jwt.decode(token).exp < Date.now() / 1000;
    if (expired) {
        console.log('Refresh token expired');
    }
    return expired;
};

const refreshAccessToken = async (refreshToken) => {
    try {
        console.log("Sending refresh token request...");
        const response = await axios.post(API_ENDPOINT + "/User/refresh-token", { refreshToken });
        console.log("Refresh token response:", response);

        if (response.status === 200) {
            const { accessToken, refreshToken: newRefreshToken } = response.data;
            console.log('New access token to decode:', accessToken);
            const user = getCurrentUser();
            user.accessToken = accessToken;
            user.refreshToken = newRefreshToken;
            localStorage.setItem("user", JSON.stringify(user));
            return user;
        }
    } catch (error) {
        console.error("Error refreshing access token: ", error);
        logout();
        // Redirect to the login page instead of reloading the current page
        window.location.href = "/login";
    }
};

const scheduleRefresh = (user) => {
    if (!user || !user.accessToken || !user.refreshToken) {
        console.log("User or tokens are missing:", user);
        return;
    }

    const decodedToken = jwt.decode(user.accessToken);
    const expiresIn = decodedToken.exp * 1000 - Date.now() - 300000; // 5 mins earlier

    console.log("Scheduling token refresh in (ms):", expiresIn);

    if (expiresIn > 0) {
        setTimeout(async () => {
            console.log("Refreshing access token...");

            try {
                const { refreshToken } = user;
                const refreshedUser = await refreshAccessToken(refreshToken);
                console.log("Access token refreshed:", refreshedUser);
                scheduleRefresh(refreshedUser);
            } catch (error) {
                console.error("Error refreshing access token: ", error);
                logout();
                window.location.href = "/login"; 
            }
        }, expiresIn);
    } else {
        console.log("Access token is already expired.");
        logout();
        window.location.href = "/login";
    }
};

axios.interceptors.request.use(
    async (config) => {
        const user = getCurrentUser();
        if (user && user.accessToken && isTokenExpired(user.accessToken)) {
            if (isRefreshTokenExpired(user.refreshToken)) {
                await logout();
                window.location.reload();
            } else {
                try {
                    const { refreshToken } = user;
                    const refreshedUser = await refreshAccessToken(refreshToken);
                    config.headers.Authorization = `Bearer ${refreshedUser.accessToken}`;
                } catch (error) {
                    console.error("Error refreshing access token: ", error);
                }
            }
        } else if (user && user.accessToken) {
            config.headers.Authorization = `Bearer ${user.accessToken}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

const register = (username, password) => {
    return axios
        .post(API_ENDPOINT + "/User/register", {
            username,
            password,
        })
        .then((response) => {
            if (response.data.accessToken) {
                localStorage.setItem("user", JSON.stringify(response.data));
            }

            return response.data;
        });
};

const login = (username, password) => {
    return axios
        .post(API_ENDPOINT + "/User/login", {
            username,
            password,
        })
        .then((response) => {
            if (response.data.accessToken) {
                localStorage.removeItem("user");

                localStorage.setItem("user", JSON.stringify(response.data));
                scheduleRefresh(response.data);
            }

            return response.data;
        });
};

const logout = () => {
    const user = getCurrentUser();
    if (user && user.refreshToken) {
        return axios
            .post(API_ENDPOINT + "/User/logout", { refreshToken: user.refreshToken })
            .then(() => {
                localStorage.removeItem("user");
            })
            .catch((error) => {
                console.error(error);
            });
    } else {
        localStorage.removeItem("user");
        return Promise.resolve();
    }
};

const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem("user"));
};

const AuthService = {
    register,
    login,
    logout,
    getCurrentUser,
};

export default AuthService;
