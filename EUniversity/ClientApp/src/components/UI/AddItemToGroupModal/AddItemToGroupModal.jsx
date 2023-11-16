import React from 'react';
import Button from '../Button/Button';
import { useState } from 'react';

const AddItemToGroupModal = ({
    isVisible,
    setIsVisible,
    title,
    groupId,
    fetchItems
}) => {

    const [inputValue, setInputValue] = useState("");
    const [foundUsers, setFoundUsers] = useState([]);
    const [chosenUsers, setChosenUsers] = useState([]);
    const [isResponsePossible, setIsResponsePossible] = useState(true);
    const [timeoutId, setTimeoutId] = useState(null);

    const handleClickOnBg = () => {
        setIsVisible(false);
        document.body.style.overflow = 'auto';
    };

    const searchItem = async e => {
        setInputValue(e.target.value);
    
        if (e.target.value.length && isResponsePossible) {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
    
            const newTimeoutId = setTimeout(async () => {
                setIsResponsePossible(false);
                try {
                    const response = await fetch(`/api/users/students?Page=1&PageSize=20&FullName=${inputValue}`);
                    
                    if (response.ok) {
                        const data = await response.json();
                        setFoundUsers(data.items);
                    } else {
                        console.log('error');
                    }
                } catch (error) {
                    console.log(error);
                } finally {
                    setIsResponsePossible(true);
                }
            }, 500);
    
            setTimeoutId(newTimeoutId);
        } else {
            setFoundUsers([]);
            setIsResponsePossible(true);
        }
    };
    

    const postNewUsersToGroup = async () => {
        const postStudents = chosenUsers.map(user => user.id);

        for (let student of postStudents) {
            try {
                const response = await fetch(`/api/groups/${groupId}/students`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: `{"studentId": ${JSON.stringify(student)}}`,
                });
                if (response.ok) {
                    console.log("ok");
                    setFoundUsers([]);
                    setChosenUsers([]);
                    setIsVisible(false);
                    await fetchItems();    
                } 
                else {
                    console.error("Error:", response.status, response.statusText);
                }
            } catch (error) {
                console.error("An error occurred:", error);
            }     
        }
    };

    const addUserToChosen = (user) => {
        if (!chosenUsers.includes(user)) {
            setChosenUsers([...chosenUsers, user]);
        }
    };

    const deleteUserFromChosen = (user) => {
        setChosenUsers(chosenUsers.filter(userFiltered => userFiltered !== user));
    };

    return (
        <div 
            onClick={handleClickOnBg}
            className={`${isVisible ? "absolute" : "hidden"} top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-30 flex items-center justify-center px-4`}
        >
            <div 
                className=" container max-w-[500px] pt-10 bg-white p-10 rounded-lg" 
                onClick={(e) => e.stopPropagation()}
            >
                <h1 className="newUser__title form__title">
                    Add {title}
                </h1>
                <div className="w-full mb-4">
                    <input 
                        className="form-control mb-4 w-full text-xl font-medium"
                        type="text" 
                        value = {inputValue}
                        onChange={searchItem}
                        placeholder="search student. . ."
                    />
                    <div className="flex flex-col gap-1 z-40 max-h-24 bottom-6 overflow-y-auto scrollbar-hide relative bg-white rounded-lg text-white shadow-lg p-2">
                        {inputValue.length 
                            ? (
                                foundUsers.map((user, index) => (
                                    <button 
                                        key={user.id} 
                                        className="text-black text-xl font-medium text-left" 
                                        onClick={() => addUserToChosen(user)}
                                    >
                                        {index + 1}. {user.firstName} {user.lastName} {user.middleName}
                                    </button>
                                ))
                            ) 
                            : (
                                <p className="text-gray-500 text-xl font-medium">no users found</p>
                            )
                        }
                    </div>
                    <div className="flex gap-1 flex-wrap">
                        {
                            chosenUsers.map(user => 
                                <div 
                                    onClick={() => deleteUserFromChosen(user)}
                                    key={user.id}
                                    className="flex gap-2 items-center bg-gray-400 px-3 py-1 rounded-full"
                                >
                                    <p className="text-gray-100 font-medium">{user.firstName} {user.lastName}</p>
                                    <button className="text-[8px] bg-gray-100 rounded-full p-0.5 text-white">✖</button>
                                </div>  
                            )
                        }
                    </div>
                </div>
                <Button onClick={postNewUsersToGroup}>Add student</Button>
            </div>
        </div>
    );
};

export default AddItemToGroupModal;