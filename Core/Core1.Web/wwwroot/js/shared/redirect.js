function redirectToAction(action, controller, params) {
    var queryString = '';

    if (params) {
        // https://howchoo.com/g/nwywodhkndm/how-to-turn-an-object-into-query-string-parameters-in-javascript
        queryString = '?' + Object.keys(params).map((key) => {
            return encodeURIComponent(key) + '=' + encodeURIComponent(params[key]);
        }).join('&');
    }

    window.location.href = `/${controller}/${action}${queryString}`;
}

function redirectToPath(path) {
    window.location.href = `${path}`;
}

export default {
    redirectToAction: redirectToAction,
    redirectToPath: redirectToPath
}