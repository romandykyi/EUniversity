import { useState } from "react"
import Button from "../UI/Button/Button";
import authorizeService from "../api-authorization/AuthorizeService";

const Login = () => {

    const [formData, setFormData] = useState({
        username: '',
        password: '',
        rememberMe: false,
    });
    const [error,setError] = useState(false);


    const handleInputChange = e => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleCheckboxChanges = e => {
        const {name, checked} = e.target;
        setFormData({
            ...formData,
            [name]: checked,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const { username, password, rememberMe } = formData;

        const requestBody = {
            username: username,
            password: password,
            rememberMe: rememberMe,
        };
        if (username && password) {
            setError(false);
            try {
                const response = await fetch("/api/auth/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(requestBody),
                });

                if (response.ok) {
                    await authorizeService.signIn();
                } else {
                    console.error("Error:", response.status, response.statusText);
                }
            } catch (error) {
                console.error("An error occurred:", error);
            }
        }
        else {
            setError(true);
        }
    };

    return (
        <div className="login">
            <div className="login__title">
                Log in to your account
            </div>
            <form onSubmit={handleSubmit} className="login__form">
                <div className="login__inputs">
                    <input
                        type="text"
                        value={formData.username}
                        className="login__input form-control"
                        placeholder="your login"
                        onChange={handleInputChange}
                        name="username"
                    />
                    <input
                        type="password"
                        value={formData.password}
                        className="login__input form-control"
                        placeholder="your password"
                        onChange={handleInputChange}
                        name="password"
                    />
                    {
                        error
                            ? <div className="login__error">
                                fill all the blanks
                              </div>
                            : ''
                    }
                    <div className="login__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={formData.rememberMe}
                            className="login__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="rememberMe"
                            id="rememberMe"
                        />
                        <label className="form-check-label" htmlFor="rememberMe">Remeber Me</label>
                    </div>
                </div>
                <Button type="submit">Log in</Button>
            </form>
        </div>
    )
};

export default Login;