import React, { Component } from "react";
import ReactDOM from "react-dom";
import DeletePerson from "./DeletePerson";

class ShowPersonDetails extends Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isLoaded: false,
      persons: []
    };
  }

  componentDidMount() {
    const URL = "/api/react/" + this.props.id;
    fetch(URL)
    .then(response => response.json())
    .then(
      (result) => {
        this.setState({
          isLoaded: true,
          persons: [result]
        });
      },
      (error) => {
        this.setState({
          isLoaded: true,
          error
        })
      },
    )
  }

  render() {
    const clickDelete = (id) => {
      ReactDOM.unmountComponentAtNode(document.getElementById('Details'));
      ReactDOM.render(<DeletePerson id={id} />, document.getElementById('Details'));
    };

    const {error, isLoaded, persons} = this.state;
    if (error) {
      return <div>Loading error!</div>
    } else if (!isLoaded) {
      return <div>Loading ...</div>
    } else {
      return (
        persons.map(person => (
          <article className="details">
            <h2>(ID: {person.PersonId}) {person.LastName}, {person.FirstName}</h2>
            <p><strong>City:</strong> {person.City}</p>
            <p><strong>Country:</strong> {person.Country}</p>
            <p><strong>Languages:</strong> {person.Languages}</p>
            <button className="delete" value={person.PersonId} onClick={e => clickDelete(e.target.value)}>Delete</button>
          </article>
        ))
      )
    }
  }

};

export default ShowPersonDetails;

