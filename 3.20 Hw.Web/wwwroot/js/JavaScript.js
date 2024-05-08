$(() => {
    setInterval(function () {
        GetLikes()
    }, 1000);
    const imageId = $("#image-id").val();

    function GetLikes() {
        $.get(`/home/getlikes?id=${imageId}`, function (likes) {
            $("#likes-count").text(likes);
        });
    }

    $("#like-button").on('click', function () {
        $.post(`/home/addlikes?id=${imageId}`, function () {
            GetLikes()
            $("#like-button").prop('disabled', true);
        });
    });
});
