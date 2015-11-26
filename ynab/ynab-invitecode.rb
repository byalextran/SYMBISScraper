# pings YNAB's download page periodically looking for an invite link to the new YNAB

require "mechanize"
require "pony"

agent = Mechanize.new
agent.user_agent_alias = 'Mac FireFox'
agent.history.max_size = 0 # disable caching

page = agent.get "http://www.youneedabudget.com/download"
link = page.link_with(:href => /invitationCode/i)
# link = page.link_with(:href => /liveCaptive/i)

unless link.nil?
  Pony.mail(:to => "EMAIL", :subject => "invitation link found", :body => link.href)
end