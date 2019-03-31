import first from 'lodash/first';

function getServerError(httpError) {
    var serverError = '';

    if (httpError.response && httpError.response.status === 400) {
        var validationErrors = error.response.data;

        serverError = first(validationErrors[Object.keys(validationErrors)[0]]);
    }
    else {
        serverError = 'Unable to complete action. Please try again later.';
    }

    return serverError;
}

export default {
    getServerError: getServerError
};