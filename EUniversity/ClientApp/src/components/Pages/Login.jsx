import { useState } from "react"
import Button from "../UI/Button/Button";

const Login = () => {

    const [formData, setFormData] = useState({
        username: '',
        password: '',
        rememberMe: false,
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
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

        try {
            const response = await fetch("/api/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(requestBody),
            });

            if (response.ok) {
                const data = await response.json();
                console.log(data);
            } else {
                console.error("Error:", response.status, response.statusText);
            }
        } catch (error) {
            console.error("An error occurred:", error);
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
                        className="login__input"
                        placeholder="your login"
                        onChange={handleInputChange}
                        name="username"
                    />
                    <input
                        type="password"
                        value={formData.password}
                        className="login__input"
                        placeholder="your password"
                        onChange={handleInputChange}
                        name="password"
                    />
                    <label htmlFor="rememberMe">Remeber Me</label>
                    <input
                        type="checkbox"
                        checked={formData.rememberMe}
                        className="login__checkbox"
                        onChange={handleInputChange}
                        name="rememberMe"
                        id="rememberMe"
                    />
                </div>
                <Button type="submit">Log in</Button>
            </form>
        </div>
    )
};

export default Login;