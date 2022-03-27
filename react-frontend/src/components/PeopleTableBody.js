import React, { Component } from "react";
import ReactDOM from "react-dom";
import ShowPersonDetails from "./ShowPersonDetails";

class PeopleTableBody extends Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isLoaded: false,
      people: []
    };
  }

  componentDidMount() {
    fetch("/api/react")
    .then(response => response.json())
    .then(
      (result) => {
        this.setState({
          isLoaded: true,
          people: result
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
    const showDetails = (id) => {
      ReactDOM.render(<ShowPersonDetails id={id} />, document.getElementById('View'));
    }
    const {error, isLoaded, people} = this.state;
    if (error) {
      return <div>Loading error!</div>
    } else if (!isLoaded) {
      return <div>Loading ...</div>
    } else {
      const rows = people.map((row, index) => {
        return (
          <tr key={index}>
            <td>{row.PersonId}</td>
            <td>{row.FirstName}</td>
            <td>{row.LastName}</td>
            <td><button className="details" value={row.PersonId} onClick={e => showDetails(e.target.value)}>Details</button></td>
          </tr>
        );
      });
      return (
        <tbody>
          {rows}
        </tbody>
      )
    }
  }

};

export default PeopleTableBody;

