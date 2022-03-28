import React from "react";
import ReactDOM from "react-dom";
import PeopleTable from "./PeopleTable";

export const SORT_METHODS = {
  NONE: "none",
  ASCENDING: "asc",
  DESCENDING: "desc"
}

const SortTable = (props) => {
  const sortAscending = () => {
    ReactDOM.unmountComponentAtNode(document.getElementById('View'));
    ReactDOM.render(<SortTable sortMethod={SORT_METHODS.ASCENDING} />, document.getElementById('View')); 
  }

  const sortDescending = () => {
    ReactDOM.unmountComponentAtNode(document.getElementById('View'));
    ReactDOM.render(<SortTable sortMethod={SORT_METHODS.DESCENDING} />, document.getElementById('View')); 
  }
  return [
    <button className="menu" onClick={e => sortAscending()}>Sort Ascending</button>,
    <button className="menu" onClick={e => sortDescending()}>Sort Descending</button>,
    <hr />,
    <PeopleTable sortMethod={props.sortMethod} />
  ];
};

export default SortTable;

