const strengthMeter = document.getElementById("strengthMeter");
const GetDataFromSignIn = () => {
    const email = document.querySelector("#email").value;
    const firstName = document.querySelector("#firstName").value;
    const lastName = document.querySelector("#lastName").value;
    const password = document.querySelector("#password").value;
    return { password, lastName, firstName, email }
}
const GetDataFromLogin = () => {
    const email = document.querySelector("#emailLogin").value;
    const password = document.querySelector("#passwordLogin").value;
    return { password , email }
}

const SignIn = async () => {
    const nweUser = GetDataFromSignIn()
    if (!nweUser.password || !nweUser.email)
        return alert("password & email are required")
    if (nweUser.firstName.length > 15 || nweUser.lastName.length > 15)
        return alert("lastName & firstName need to be between 0 till 15")
    try {
        const responsePost = await fetch("api/users", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(nweUser)
        })
        const dataPost = await responsePost.json();
        if (!responsePost.ok)
            alert("password is not enough strong , please enter a difference..")//alerts in any error, e.g.- email address is not valid.
            //need to have different alerts to each error
        else
            alert(`${dataPost.firstName} created`)
    }
    catch (error) {
        alert("error")
    }
}
const Login = async () => {
    const newUser = GetDataFromLogin()
    try {
        const responsePost = await fetch(`api/users/login/?email=${newUser.email}&password=${newUser.password}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            query: { email: newUser.email, password: newUser.password }
        })
        if (responsePost.status === 204)
            return alert("user not found")
        const dataPost = await responsePost.json();
        alert(`welcome ${dataPost.firstName}`)
        window.location.href = "updateUserDetails.html"
        sessionStorage.setItem("currentUser", dataPost.userId)
    }
    catch (err) {
            alert("")
    } 
}

const ShowRegister = () => {
    const register = document.querySelector(".visibleRegister");
    register.classList.remove("visibleRegister")
}
const CheckPassword = async() => {
    const cPassword = document.querySelector("#password").value;
    try {
        const responsePost = await fetch(`api/users/password/?password=${cPassword}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            query: { password: cPassword }
        });
        const dataPost = await responsePost.json();
        strengthMeter.value = dataPost;
        if (responsePost.status === 400)
            return alert("password is too weak!")
        else
            return alert("password is strong enough")
    }
    catch (err) {
        alert("")
    }
}

