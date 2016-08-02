Sys.Application.add_load(function () {
    var editor1 = KindEditor.create('#ctl00_ContentPlaceHolder1_CustomsEditor_editor', {
        cssPath: '../KindEditor/plugins/code/prettify.css',
        uploadJson: '../KindEditor/upload_json.ashx',
        fileManagerJson: '../KindEditor/file_manager_json.ashx',
        allowFileManager: true,
        items: [
		'source', '|', 'undo', 'redo', '|', 'preview', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
		'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
		'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
		'table', 'hr', 'emoticons', 'baidumap', 'anchor', 'link', 'unlink', 'insertfile'
	],
        afterBlur: function () {
            this.sync();
            $('#ctl00_ContentPlaceHolder1_CustomsEditor_hf_EditorValue').html($('#ctl00_ContentPlaceHolder1_CustomsEditor_editor').html());
        }
    });
});