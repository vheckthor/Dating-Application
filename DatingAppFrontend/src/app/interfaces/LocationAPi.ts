 export function locationSuccess(position) {
    const latitude = position.coords.latitude;
    const longitude = position.coords.longitude;
    const altitude = position.coords.altitude;
    const accuracy = position.coords.accuracy;
    const altitudeAccuracy = position.coords.altitudeAccuracy;
    const heading = position.coords.height;
    const speed = position.coords.speed;
    const timestamp = position.timestamp;

    return {
        latitude,
        longitude,
        altitude,
        accuracy,
        altitudeAccuracy,
        heading,
        speed,
        timestamp
    }

    // work with this information however you'd like!
    }

 export function locationError(error) {
    const code = error.code;
    const message = error.message;
  // read the code and message and decide how you want to handle this!
}

 export function value() {
   return navigator.geolocation.getCurrentPosition(locationSuccess, locationError);
}