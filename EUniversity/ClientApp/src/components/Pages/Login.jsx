import { useState } from "react"
import Button from "../UI/Button/Button";
import authorizeService from "../api-authorization/AuthorizeService";

import {useNavigate} from "react-router-dom";

const Login = () => {

    const [formData, setFormData] = useState({
        username: '',
        password: '',
        rememberMe: false,
    });
    const [error,setError] = useState('');
    const [isPasswordShowed, setIsPasswordShowed] = useState(false);
    const navigate = useNavigate();

    const handleInputChange = e => {
        setError('');
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleCheckboxChanges = e => {
        const {name, checked} = e.target;
        if (name === "showPassword") {
            setIsPasswordShowed(prevState => !prevState);
        }
        else {
            setFormData({
                ...formData,
                [name]: checked,
            });
        }
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
            setError('');
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
                    navigate('/');
                } else {
                    console.error("Error:", response.status, response.statusText);
                    if (response.status === 401) {
                        setError('incorrect login or password');
                    }
                }
            } catch (error) {
                console.error("An error occurred:", error);
            }
        }
        else {
            setError('fill all the blanks!');
        }
    };

    return (
        <div className="login form">
            <div className="login__title form__title">
                Log in to your account
            </div>
            <form onSubmit={handleSubmit} className="login__form form__form">
                <div className="login__inputs form__inputs">
                    <input
                        type="text"
                        value={formData.username}
                        className="login__input form__input form-control"
                        placeholder="your login"
                        onChange={handleInputChange}
                        name="username"
                    />
                    <input
                        type={isPasswordShowed ? "text" : "password"}
                        value={formData.password}
                        className="login__input form__input form-control"
                        placeholder="your password"
                        onChange={handleInputChange}
                        name="password"
                        style={{
                            marginBottom: 4
                        }}
                    />
                    <div className="login__checkboxDiv form__checkboxDiv" style={{ marginBottom: 16 }}>
                        <input
                            type="checkbox"
                            checked={isPasswordShowed}
                            className="login__checkbox form__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="showPassword"
                            id="showPassword"
                        />
                        <label className="form-check-label" htmlFor="showPassword">show password</label>
                    </div>
                    <div className="login__checkboxDiv form__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={formData.rememberMe}
                            className="login__checkbox form__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="rememberMe"
                            id="rememberMe"
                        />
                        <label className="form-check-label" htmlFor="rememberMe">Remeber Me</label>
                    </div>
                </div>
                <div className="login__error form__error">
                    {error}
                </div>
                <Button type="submit">Log in</Button>
            </form>
        </div>
    )
};

export default Login;