import React from "react";
import ReactDOM from "react-dom";
import ShowPersonDetails from "./ShowPersonDetails";
import { PeopleArticle } from "./StyledComponents";

export const ShowPerson = (props) => {
  const ShowDetails = (id) => {
    ReactDOM.render(<ShowPersonDetails id={id} />, document.getElementById('View'));
  }

  return (
    <PeopleArticle>
      <h2><button className="details" value={props.item.PersonId} onClick={e => ShowDetails(e.target.value)}>Details</button> (ID: {props.item.PersonId}) {props.item.LastName}, {props.item.FirstName}</h2>
    </PeopleArticle>
  );
};

