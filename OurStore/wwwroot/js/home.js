let resultPassword=0;
const GetDataFromSignIn = () => {
    const email = document.querySelector("#email").value;
    const firstName = document.querySelector("#firstName").value;
    const lastName = document.querySelector("#lastName").value;
    const password = document.querySelector("#password").value;
    return ({ email, password, firstName, lastName })
}
const GetDataFromLogin = () => {
    const email = document.querySelector("#emailLogin").value;
    const password = document.querySelector("#passwordLogin").value;
    return { password , email }
}

const SignIn = async () => {
    const nweUser = GetDataFromSignIn()
    if (!nweUser.email || !nweUser.password || !nweUser.firstName)
        alert("Fields are required")
    else if (nweUser.email.indexOf('@') == -1 || nweUser.email.indexOf('@') == nweUser.email.length-1)
        alert("Invalid email address")
    else if (resultPassword < 3) 
        alert("weak password")
    else if (nweUser.firstName.length < 2 || nweUser.firstName.length > 20)
        alert("Name can be between 2 till 20 letters")
    else {
        try {
            const responsePost = await fetch("api/users", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(nweUser)
            })
            const dataPost = await responsePost.json();
            if (responsePost.ok) {
                alert(`${dataPost.firstName} created`)
                window.location.href ="home.html"
            }
        }
        catch (error) {
            alert(error)
        }
    }
}
const Login = async () => {
    const newUser = GetDataFromLogin()
    if (!newUser.email || !newUser.password)
        alert("Fields are required")
    else {
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
            window.location.href = "Products.html"
            sessionStorage.setItem("currentUser", dataPost.id)
        }
        catch (err) {
            alert(err)
        }
    }
}


const CheckPassword = async () => {
    const password = document.querySelector("#password").value;
    if (password) {
        try {
            const responsePost = await fetch(`api/users/password/?password=${password}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                query: { password: password }
            });
            const dataPost = await responsePost.json();
            document.getElementById("strengthMeter").value= dataPost;
            resultPassword = dataPost
        }
        catch (err) {
            alert(err)
        }
    }
}
const updateDetails = async () => {
    const nweUser = GetDataFromSignIn()
    if (!nweUser.email || !nweUser.password || !nweUser.firstName)
        alert("Fields are required")
    else if (nweUser.email.indexOf('@') == -1 || nweUser.email.indexOf('@') == nweUser.email.length - 1)
        alert("Invalid email address")
    else if (resultPassword < 3)
        alert("weak password")
    else if (nweUser.firstName.length < 2 || nweUser.firstName.length > 20)
        alert("Name can be between 2 till 20 letters")
    else {
        try {
            const id = sessionStorage.getItem("currentUser")
            const responsePut = await fetch(`api/users/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(nweUser)
            })
            if (responsePut.ok) {
                alert(`updated succefully`)
                window.location.href ="Products.html"
            }
        }
        catch (err) {
            alert(err)
        }
    }
}
    const details = async () => {
        try {
            const id = sessionStorage.getItem("currentUser")
            console.log(id)
            const responseGet = await fetch(`api/users/${id}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                }
            })

            if (responseGet.ok) {
                detailsUser = await responseGet.json()
            }
            document.querySelector("#email").value = detailsUser.email
            document.querySelector("#firstName").value = detailsUser.firstName
            document.querySelector("#lastName").value = detailsUser.lastName
        }
        catch (err) {
            alert(err)
        }
        
}
const onNewUserClick = () => {
    const loginForm = document.querySelector('.loginForm'); // טופס הכניסה
    const signInForm = document.querySelector('.signIn'); // טופס הרישום
    // מסתיר את טופס הכניסה ומציג את טופס הרישום
    loginForm.style.display = "none";
    signInForm.style.display = "block";
}
