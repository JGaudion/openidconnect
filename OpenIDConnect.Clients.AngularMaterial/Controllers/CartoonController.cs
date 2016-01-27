﻿using System.Collections.Generic;
using System.Web.Http;
using OpenIDConnect.Clients.AngularMaterial.Models;

namespace OpenIDConnect.Clients.AngularMaterial.Controllers
{
    public class CartoonController : ApiController
    {

        [HttpGet]
        [Route("api/cartoons")]
        public IHttpActionResult GetCartoons()
        {
            var cartoons = new List<Cartoon>()
            {
                new Cartoon() {Id = 1, Title = "Thundercats" , Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 2,Title = "Transformers",Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 3,Title = "Teenage Mutant Hero Turtles", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 4,Title = "Animals of Farthing Wood", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 5,Title = "Duck Tales", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 6,Title = "Pirates of Dark Water", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 7,Title = "Poddington Peas",Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 8,Title = "Sharkey and George",Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 9,Title = "Peter Pan and the Pirates", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 10,Title = "Tailspin", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 11,Title = "Pinky and the Brain", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 12,Title = "Animaniacs", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 13,Title = "Dogtanian", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } },
                new Cartoon() {Id = 14,Title = "Dungeons and Dragons", Species= new List<string>() { "WildCat", "Lion", "Cheetah", "Tiger", "Panther", "Mummy", "Jackal", "Toad" } }
            };
            return Ok(cartoons);

        }
    }
}