import React from 'react';

const SortSelect = ({ setSortingMethod }) => {
    return (
        <select className="form-select" onChange={e => setSortingMethod(parseInt(e.target.value))}>
            <option defaultValue disabled>Sort by:</option>
            <option value="0">Default</option>
            <option value="1">Name</option>
            <option value="2">Descending</option>
            <option value="3">Newest</option>
            <option value="4">Oldest</option>
        </select>
    );
};

export default SortSelect;