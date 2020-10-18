import React, { useState, useEffect } from "react";
import getUrls from "get-urls";
import {
  Grid,
  Image,
  Input,
  Icon,
  InputOnChangeData,
  Button,
  TextArea,
  Form,
  List,
  Checkbox,
} from "semantic-ui-react";

export const DownloadMain = () => {
  const [searchUrl, setsearchUrl] = useState(
    "https://stocksnap.io/search/wallpaper"
  );
  const [pageResponse, setPageResponse] = useState("");
  const [imagesUrls, setImagesUrls] = useState([""]);
  const [showHtml, setShowHtml] = useState(false);
  const [showImagesUrls, setShowImagesUrls] = useState(false);
  const [imagesRows, setImagesRows] = useState(Array<JSX.Element>());

  useEffect(() => {
    if (imagesUrls.length > 1) showImages();
  }, [imagesUrls]);
  //
  const imageRow = (image1: string, image2: string, image3: string) => (
    <Grid.Row columns={3}>
      <Grid.Column>
        <Image src={image1 ?? "/image.png"} />
      </Grid.Column>
      <Grid.Column>
        <Image src={image2 ?? "/image.png"} />
      </Grid.Column>
      <Grid.Column>
        <Image src={image3 ?? "/image.png"} />
      </Grid.Column>
    </Grid.Row>
  );

  const showImages = () => {
    debugger;
    var images = Array<string>();
    var rows = Array<JSX.Element>();
    if (imagesUrls.length <= 1) return rows;
    imagesUrls.map((name, index) => {
      debugger;
      if (index != imagesUrls.length - 1 && index % 3 !== 2) {
        images.push(name);
        return;
      }
      images.push(name);
      rows.push(imageRow(images[0], images[1], images[2]));
      images = Array<string>();
    });

    setImagesRows(rows);
  };

  const searchUrlChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    data: InputOnChangeData
  ) => {
    setsearchUrl(data.value);
  };

  let headers = {
    // "content-type": "application/octet-stream",
    // accept: "application/json",
    //"Access-Control-Allow-Origin": "*",
    //origin: window.location.origin.toString(),
    // useQueryString: true,
  };


  const search = () => {
    //debugger;
    //"https://api.github.com/users/mapbox"
    //var config = new AxiosRequestConfig(){}
  };

  const getImageUrls = () => {
    var urls = getUrls(pageResponse);
    var images = Array.from(urls).filter(
      (a) =>
        a.indexOf(".jpg") != -1 ||
        a.indexOf(".png") != -1 ||
        a.indexOf(".gif") != -1 ||
        a.indexOf(".jpeg") != -1
    );
    setImagesUrls(images);
  };

  return (
    <div>
      <div style={{ height: "20px", maxWidth: "20px" }}></div>
      <Grid style={{ margin: "20px" }}>
        <Input
          icon={<Icon name="search" inverted circular link onClick={search} />}
          value={searchUrl}
          onChange={searchUrlChange}
          placeholder="Search..."
        />
        <Input
          type="text"
          placeholder="Search..."
          action
          value={searchUrl}
          onChange={searchUrlChange}
        >
          <input defaultValue="http://ww.short.url/c0opq" />
          <Button onClick={search}>Search</Button>
        </Input>

        <Button onClick={getImageUrls}>Get Pages</Button>
        <Checkbox
          label="Show Html"
          defaultChecked={showHtml}
          onChange={() => {
            setShowHtml(!showHtml);
          }}
        ></Checkbox>
        <Checkbox
          label="Show Image Urls"
          defaultChecked={showImagesUrls}
          onChange={() => {
            setShowImagesUrls(!showImagesUrls);
          }}
        ></Checkbox>
        <Grid.Row columns={2} textAlign="justified">
          <Grid.Column hidden={!showHtml}>
            <Form>
              <TextArea
                rows={20}
                placeholder="page response"
                value={pageResponse}
                style={{ maxWidth: "500" }}
              />
            </Form>
          </Grid.Column>
          <Grid.Column hidden={!showImagesUrls}>
            <List ordered>
              {imagesUrls.map((name) => (
                <List.Item>{name}</List.Item>
              ))}
            </List>
          </Grid.Column>
        </Grid.Row>

        {imagesRows}
      </Grid>
    </div>
  );
};
