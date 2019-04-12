let currentUser = undefined;

if (document.getElementById('CurrentUser')) {
    currentUser = JSON.parse(document.getElementById('CurrentUser').innerHTML);
}

export default currentUser;