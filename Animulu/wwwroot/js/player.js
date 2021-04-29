var player;
var paused = true;

function startVideo() {
    var video;
    var url = document.getElementById("videoContainer").getAttribute('value');
    video = document.querySelector("video");
    player = dashjs.MediaPlayer().create();
    player.initialize(video, url, false);
    var controlbar = new ControlBar(player);
    controlbar.initialize();
}

document.addEventListener("DOMContentLoaded", function () {
    startVideo();
});

document.addEventListener('keydown', function (event) {
    if (event.keyCode == 32 && event.target == document.body) {
        if (paused) {
            event.preventDefault();
            paused = false;
            player.play();
        }
        else {
            event.preventDefault();
            paused = true;
            player.pause();
        }
    }
});