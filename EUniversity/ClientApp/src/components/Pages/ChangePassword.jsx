import { useState } from "react"
import Button from "../UI/Button/Button";

import {useNavigate} from "react-router-dom";

// TT DO:
// do a show password button

const ChangePassword = () => {

    const [formData, setFormData] = useState({
        currentPassword: '',
        newPassword: '',
    });

    const [error,setError] = useState(false);
    const [isPasswordCahnged, setIsPasswordCahnged] = useState(false);
    const [checkBoxes, setCheckBoxes] = useState({
       checkbox1: false,
       checkbox2: false,
    });
    const navigate = useNavigate();

    const handleInputChange = e => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleCheckboxChanges = e => {
        const {name, checked} = e.target;
        setCheckBoxes({
            ...formData,
            [name]: checked,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const { currentPassword, newPassword } = formData;


        const requestBody = {
            currentPassword: currentPassword,
            newPassword: newPassword,
        };

        if (newPassword && currentPassword) {
            setError(false);
            try {
                const response = await fetch("/api/auth/password/change", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(requestBody),
                });

                if (response.ok) {
                    setIsPasswordCahnged(true);
                    navigate('/');
                    setIsPasswordCahnged(false);
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
        <div className="changePassword">
            <div className="changePassword__title">
                Change your password
            </div>
            <form onSubmit={handleSubmit} className="changePassword__form">
                <div className="changePassword__inputs">
                    <input
                        type={checkBoxes.checkbox1 ? "text" : "password"}
                        value={formData.username}
                        className="changePassword__input form-control"
                        placeholder="current password"
                        onChange={handleInputChange}
                        name="currentPassword"
                    />
                    <div className="changePassword__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={checkBoxes.checkbox1}
                            className="changePassword__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="checkbox1"
                            id="showPassword1"
                        />
                        <label className="form-check-label" htmlFor="showPassword1">show password</label>
                    </div>
                    <input
                        type={checkBoxes.checkbox2 ? "text" : "password"}
                        value={formData.password}
                        className="changePassword__input form-control"
                        placeholder="new password"
                        onChange={handleInputChange}
                        name="newPassword"
                    />
                    <div className="changePassword__checkboxDiv">
                        <input
                            type="checkbox"
                            checked={checkBoxes.checkbox2}
                            className="changePassword__checkbox form-check-input"
                            onChange={handleCheckboxChanges}
                            name="checkbox2"
                            id="showPassword2"
                        />
                        <label className="form-check-label" htmlFor="showPassword2">show password</label>
                    </div>
                    {
                        error
                            ? <div className="changePassword__error">
                                fill all the blanks
                            </div>
                            : ''
                    }

                </div>
                <Button type="submit">Change password</Button>
            </form>
        </div>
    )
};

export default ChangePassword;