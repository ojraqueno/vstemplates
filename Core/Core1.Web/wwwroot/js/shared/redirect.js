function redirectToAction(action, controller) {
    window.location.href = `/${controller}/${action}`;
};

function redirectToPath(path) {
    window.location.href = `${path}`;
};

export default {
    redirectToAction: redirectToAction,
    redirectToPath: redirectToPath
};