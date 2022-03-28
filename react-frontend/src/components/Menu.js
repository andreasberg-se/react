import React from "react";
import ReactDOM from "react-dom";
import PeopleDisplay from "./PeopleDisplay";
import SortTable, { SORT_METHODS } from "./SortTable";
import AddPersonForm from "./AddPersonForm";

export const MENU_OPTIONS = {
  TABLE_VIEW: "table_view",
  LIST_VIEW: "list_view",
  ADD_PERSON: "add_person"
}

function menuClick(menuOption) {
  switch(menuOption) {
    case MENU_OPTIONS.TABLE_VIEW:
      ReactDOM.unmountComponentAtNode(document.getElementById('View'));
      ReactDOM.render(<SortTable sortMethod={SORT_METHODS.NONE} />, document.getElementById('View'));    
      break;

    case MENU_OPTIONS.LIST_VIEW:
      ReactDOM.unmountComponentAtNode(document.getElementById('View'));
      ReactDOM.render(<PeopleDisplay />, document.getElementById('View'))
      break;

    case MENU_OPTIONS.ADD_PERSON:
      ReactDOM.unmountComponentAtNode(document.getElementById('View'));
      ReactDOM.render(<AddPersonForm />, document.getElementById('View'));
      break;

    default:
      break;
  }
}

export default menuClick;

