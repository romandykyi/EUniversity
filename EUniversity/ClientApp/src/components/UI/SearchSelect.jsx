import { React, useState } from 'react';

const SearchSelect = ({
    handleInputChange,
    itemId,
    link,
    title
}) => {

    const [isResponsePossible, setIsResponsePossible] = useState(true);
    const [timeoutId, setTimeoutId] = useState(null);
    const [inputValue, setInputValue] = useState("");
    const [foundItems, setFoundItems] = useState([]);
    const [chosenItem, setChosenItem] = useState({});
    const [isUser, setIsUser] = useState(false);
    

    const setItemLikeChosen = (item) => {
        setChosenItem(item);
        const id = item.id;
        if (item.firstName) {
            setInputValue(`${item.firstName} ${item.lastName}`);
            setFoundItems([]);
            handleInputChange(itemId, 'teacher', id);
        }
        else if (item.name) {
            setInputValue(`${item.name}`);
            setFoundItems([]);
            handleInputChange(itemId, 'course', id);
        }
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
                    const response = await fetch(`${link}${inputValue}`);
                    
                    if (response.ok) {
                        const data = await response.json();
                        if (data.items[0].firstName) setIsUser(true);
                        setFoundItems(data.items);
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
            setFoundItems([]);
            setIsResponsePossible(true);
        }
    };

    return (
        <>
            <input 
                className="form-control mb-4 w-full text-xl font-medium"
                type="text" 
                value = {inputValue}
                onChange={searchItem}
                placeholder={`search ${title}. . .`}
            />
            {
                inputValue === (isUser ? `${chosenItem.firstName} ${chosenItem.lastName}` : `${chosenItem.name}`)
                ?   ""
                :   <div className="flex flex-col gap-1 z-40 max-h-24 bottom-6 overflow-y-auto scrollbar-hide relative bg-white rounded-lg text-white shadow-lg p-2">
                        {inputValue.length
                            ? (
                                foundItems.map((item, index) => (
                                    <button 
                                        key={item.id} 
                                        className="text-black text-xl font-medium text-left" 
                                        onClick={() => setItemLikeChosen(item)}
                                    >
                                        {
                                            isUser
                                            ? `${index + 1}. ${item.firstName} ${item.lastName}`
                                            : `${index + 1}. ${item.name}`
                                        }
                                    </button>
                                ))
                            ) 
                            : (
                                <p className="text-gray-500 text-xl font-medium">no users found</p>
                            )
                        }
                    </div>   
            }
        </>
    );
};

export default SearchSelect;