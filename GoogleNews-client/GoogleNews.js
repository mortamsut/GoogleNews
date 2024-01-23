loadTopics();
// Presentation of the subjects
function loadTopics()
{
  axios.get("http://localhost:5001/")
.then((response) => {
  console.log(response.data)
  var topics= document.getElementById("topic");
  var topiclist = document.getElementById("topicList");

  for(var item of response.data)
  {
    var li = document.createElement('li');
    var topicItem = document.createElement('button');
    topicItem.setAttribute('id', item.id); 

     //on click load the post
    topicItem.onclick = function(event){
      loadPost(event)}
    topicItem.textContent = item.title;
    li.appendChild(topicItem);
    topiclist.appendChild(li);
   
  }
  topics.appendChild(topiclist);
})
.catch((error) => console.log(error));
}

//Load the post with AJAX 
function loadPost(event) {
 
      var id = event.target.id;
      const xhttp = new XMLHttpRequest();
      //Id separation
      const regex = /(?:\?|&)p=([^&]+)/;
      const match = id.match(regex);  
      const idValue = match ? match[1] : null;

      xhttp.onload = function() {
      var response = JSON.parse(this.response);
        showPost(response);
    }
      // Get element by id
      xhttp.open("GET", "http://localhost:5001/"+idValue);
      xhttp.send();
  
}
//show post function
function showPost(response)
{
  console.log(this.response);
  var postElement = document.getElementById("post");

  var titleElement = document.createElement("h4");
  titleElement.textContent = response.title;

  
  var descriptionElement = document.createElement("div");
  descriptionElement.innerHTML=response.description;

  var linkElement = document.createElement("a");
  linkElement.href = response.link;
  linkElement.textContent = "Read more";

  postElement.innerHTML = "";

  postElement.appendChild(titleElement);
  postElement.appendChild(descriptionElement);
  postElement.appendChild(linkElement);
}



