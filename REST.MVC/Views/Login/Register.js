(function() {
    // $('#register-form').submit(function(event) {
    //     event.preventDefault();

    //     var data = utils.objectifyFormData('#register-form');

    //     var errsSection = $('.errors');
    //     errsSection.addClass('hidden').find('.errorsList').empty();

    //     $.ajaxSetup({
    //         contentType: 'application/json'
    //     });
    //     $.post('/api/login/signup', JSON.stringify(data), function(elData) {
    //         $('.success').removeClass('hidden');
    //         $('.success .successList').append(`<li>You have been successfully registered.</li>
    //         <li>You will be automatically redirected to login or you can <a href="/Login" title="Login">click here</a> to redirect now.</li>`);

    //         setTimeout(function() {
    //             window.location(`/Login?userName=${data[0].value}`);
    //         }, 2500);
    //     }, 'json')
    //         .fail(function(err) {
    //             if (err.status === 400) {
    //                 errsSection.removeClass('hidden');
    //                 console.error(err);
    //                 for (var key in err.responseJSON) {
    //                     errsSection.find('.errorsList').append(`<li>${err.responseJSON[key][0]}</li>`)
    //                 }
    //             }
    //         });

    // });
    $('#register-form').validate({
        errorClass: 'error invalid-feedback'
    });
})();