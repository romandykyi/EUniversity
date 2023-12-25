import { React, useEffect, useState } from 'react';

const SearchSelect = ({
    handleInputChange,
    itemId,
    link,
    title,
    givenValue
}) => {

    const [isResponsePossible, setIsResponsePossible] = useState(true);
    const [timeoutId, setTimeoutId] = useState(null);
    const [inputValue, setInputValue] = useState('');
    const [foundItems, setFoundItems] = useState([]);
    const [chosenItem, setChosenItem] = useState({});
    const [isUser, setIsUser] = useState(false);
    const [inputChangeCount, setInputChangeCount] = useState(0);

    useEffect(() => {
        setInputChangeCount(0);
    }, [])
    
    useEffect(() => {
        if (!inputChangeCount) setInputValue(givenValue);
        setFoundItems([]);
    }, [givenValue]);

    useEffect(() => {
        setInputValue(givenValue);
    }, [itemId]);
    

    const setItemLikeChosen = (item) => {
        setChosenItem(item);
        const id = item.id;
        if (item.firstName) {
            setInputChangeCount(inputChangeCount + 1);
            setInputValue(`${item.firstName} ${item.lastName}`);
            setFoundItems([]);
            handleInputChange(itemId, 'teacher', id);
        }
        else if (item.name) {
            setInputChangeCount(inputChangeCount + 1);
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
                className="form-control w-full text-xl font-medium min-w-[200px] bg-background text-text focus:bg-background focus:text-text placeholder:text-text"
                type="text" 
                value = {inputValue}
                onChange={searchItem}
                placeholder={`search ${title}. . .`}
            />
             {
                inputValue === (isUser ? `${chosenItem.firstName} ${chosenItem.lastName}` : `${chosenItem.name}`)
                ?   ""
                :   <div className="flex flex-col gap-1 z-10 max-h-24 overflow-y-auto scrollbar-hide absolute max-w-[353px] w-full bg-background rounded-lg text-text shadow-lg p-2">
                        {inputValue
                            ?   foundItems.length
                                ? foundItems.map((item, index) => (
                                    <button 
                                        key={item.id} 
                                        className="text-text text-xl font-medium text-left" 
                                        onClick={() => setItemLikeChosen(item)}
                                    >
                                        {
                                            isUser
                                            ? `${index + 1}. ${item.firstName} ${item.lastName}`
                                            : `${index + 1}. ${item.name}`
                                        }
                                    </button>
                                ))
                                : <p className="text-gray-500 text-xl font-medium">no items found</p>
                            :   ""
                        }
                    </div>   
            }
        </>
    );
};

export default SearchSelect;