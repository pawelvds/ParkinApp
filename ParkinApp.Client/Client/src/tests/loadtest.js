import http from 'k6/http';
import { check, group, sleep } from 'k6';

export let options = {
    vus: 1,
    iterations: 1,
    duration: '10s',
};

const tokens = [
    `eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJkcmFrZSIsIm5iZiI6MTY4MzAxOTAwOSwiZXhwIjoxNjgzMDE5OTA5LCJpYXQiOjE2ODMwMTkwMDl9.Fav71feuFAO0LvQGE0BukNHKx-IqHPc6P1tpywd4tEhZ6q7nJILartgwHVIqW8pAYCAKAo87lK6aO0xHu96YMQ`,
    `eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJyZW1payIsIm5iZiI6MTY4MzAxOTA0NSwiZXhwIjoxNjgzMDE5OTQ1LCJpYXQiOjE2ODMwMTkwNDV9.mnAkK8xj4ngi1S12ICOmWwek2TQ3zbHbFE0zR1zhIFLL_ZOTgEbn6yWN82kIZGQiQJ9FuR_uMfdH_MKIVumKpQ`,
    `eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJ0b20iLCJuYmYiOjE2ODMwMTkwNjYsImV4cCI6MTY4MzAxOTk2NiwiaWF0IjoxNjgzMDE5MDY2fQ.7OKNb7N_M8GWcNFBCeLtG5OtjDy8j9ZyFDfnM85bLxsDhtf7fu8SevUkTyCG0OytMOrBWT0jGYIXe3wOjPAbjg`,
];

const reservationUrl = 'http://localhost:5169/api/Reservations/create';
const cancelReservationUrl = 'http://localhost:5169/api/Reservations/cancel';

function makeReservation(token) {
    const payload = JSON.stringify({
        parkingSpotId: 16,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
    };

    const res = http.post(reservationUrl, payload, params);
    const isSuccess = check(res, {
        'status was 200': (r) => r.status == 200,
    });

    if (isSuccess) {
        console.log(`Reservation successful for user with token: ${token}`);
        return true;
    }
    return false;
}

function cancelReservation(token) {
    const params = {
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    };

    const res = http.del(cancelReservationUrl, {}, params);
    check(res, {
        'status was 200': (r) => r.status == 200,
    });
}

export default function () {
    let successfulUserToken;

    for (const token of tokens) {
        group(`User with token: ${token}`, () => {
            if (!successfulUserToken && makeReservation(token)) {
                successfulUserToken = token;
            }
        });
    }

    if (successfulUserToken) {
        console.log(`Cancelling reservation for user with token: ${successfulUserToken}`);
        cancelReservation(successfulUserToken);
    }
    sleep(1);
}
