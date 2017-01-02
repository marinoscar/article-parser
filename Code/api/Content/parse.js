$(function () {
    $(document).ajaxSend(function (event, request, settings) {
        $('#spinner').removeClass('hidden');
    });

    $(document).ajaxComplete(function (event, request, settings) {
        $('#spinner').addClass('hidden');
    });

    $('#btn-parse').on('click', function () {
        parseUrl();
    });
});

function parseUrl() {
    var url = $('#txt-parse').val();
    callService(url);
}

function callService(url) {
    $.ajax({
        url: '/api/parser/?url=' + encodeURI(url),
        dataType: 'json',
        beforeSend: function (xhr, settings) {
            xhr.setRequestHeader('token', 'b5d4830dc7fc4881b9a3e89154f86a20b911b5fb33ef4cf99672ba2f82dbb42ac5d79008e10d4ab187f40aded3d7f29db51e78bf654a4f24a5f62415b9c334e1');
        },
        success: function (data, textStatus, xhr) {
            loadContent(data);
        },
        error: function (xhr, textStatus, errorThrown) {
            $('#doc').addClass('hidden');
            $('#error').removeClass('hidden');
            $('#error-content').html('<p>' + xhr.responseJSON.Message + '</p>')
        }
    });
}

function loadContent(item) {
    var keyHtml = '';
    var catHtml = '';
    $('#article-title').html(item.Title);
    $('#article-author').html(item.Author);
    $('#article-url').html(item.Url).attr('href', item.Url);
    $('#article-content').html(item.FormattedContent);
    for (i = 0; i < item.Keywords.length; i++) {
        keyHtml += '<span class="label label-success">' + item.Keywords[i] + '</span><span style="padding-left:5px;"></span>';
    }
    for (i = 0; i < item.Categories.length; i++) {
        catHtml += '<span class="label label-info" >' + item.Categories[i] + '</span></span><span style="padding-left:5px;"></span>';
    }
    $('#panel-cat').html(catHtml);
    $('#panel-key').html(keyHtml);
    $('#doc').removeClass('hidden');
    $('#error').addClass('hidden');
}