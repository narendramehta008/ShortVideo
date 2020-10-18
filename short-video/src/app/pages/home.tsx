import React from "react";

export default class Home extends React.Component {
  componentDidMount() {
    debugger;
    var val = process.env;
  }

  render() {
    return (
      <div>
        <h1>
          {process.env.NODE_ENV} Inside home {process.env.LOGIN_API}
        </h1>
      </div>
    );
  }
}
