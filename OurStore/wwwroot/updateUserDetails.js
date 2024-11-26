const GetDataFromUpdate=() => {
    const email = document.querySelector("#emailUpdate").value;
    const firstName = document.querySelector("#firstNameUpdate").value;
    const lastName = document.querySelector("#lastNameUpdate").value;
    const password = document.querySelector("#password").value;
    return { password, lastName, firstName, email }
}
const updateDetails = async () => {
    const newUser = GetDataFromUpdate()
    if (!newUser.password || !newUser.email)
        return alert("password & email are required")
    if (newUser.firstName.length > 15 || newUser.lastName.length > 15)
        return alert("lastName & firstName need to be between 0 till 15")
    try {
        const id = sessionStorage.getItem("currentUser")
        const responsePut = await fetch(`api/users/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newUser)
        })
        const dataPut = await responsePut.json();//יש נפילה בשורה הזאת למרות שהעדכון עצמו עובד מצוין
        if (responsePut.ok)
            alert(`${dataPut.firstName} updated`)
        else 
            alert("password is not enough strong , please enter a difference..")
            
    }
    catch (a) {
        alert("")
    }
 }
